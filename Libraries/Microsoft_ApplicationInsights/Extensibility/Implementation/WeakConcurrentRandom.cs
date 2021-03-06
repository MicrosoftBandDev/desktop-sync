// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.WeakConcurrentRandom
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal class WeakConcurrentRandom
  {
    private static WeakConcurrentRandom random;
    private int index;
    private int segmentCount;
    private int segmentSize;
    private int bitsToStoreRandomIndexWithinSegment;
    private int segmentIndexMask;
    private int randomIndexWithinSegmentMask;
    private int randomArrayIndexMask;
    private IRandomNumberBatchGenerator[] randomGemerators;
    private ulong[] randomNumbers;

    public WeakConcurrentRandom() => this.Initialize();

    public static WeakConcurrentRandom Instance
    {
      get
      {
        if (WeakConcurrentRandom.random != null)
          return WeakConcurrentRandom.random;
        Interlocked.CompareExchange<WeakConcurrentRandom>(ref WeakConcurrentRandom.random, new WeakConcurrentRandom(), (WeakConcurrentRandom) null);
        return WeakConcurrentRandom.random;
      }
    }

    public void Initialize() => this.Initialize((Func<ulong, IRandomNumberBatchGenerator>) (seed => (IRandomNumberBatchGenerator) new XorshiftRandomBatchGenerator(seed)), 3, 10);

    public void Initialize(
      Func<ulong, IRandomNumberBatchGenerator> randomGeneratorFactory,
      int segmentIndexBits,
      int segmentBits)
    {
      int num1 = segmentIndexBits;
      if (segmentIndexBits < 1 || segmentIndexBits > 4)
        num1 = 3;
      int num2 = segmentBits;
      if (segmentBits < 7 || segmentBits > 15)
        num2 = 9;
      this.bitsToStoreRandomIndexWithinSegment = num2;
      this.segmentCount = 1 << num1;
      this.segmentSize = 1 << num2;
      this.segmentIndexMask = this.segmentCount - 1 << this.bitsToStoreRandomIndexWithinSegment;
      this.randomIndexWithinSegmentMask = this.segmentSize - 1;
      this.randomArrayIndexMask = this.segmentIndexMask | this.randomIndexWithinSegmentMask;
      int count = this.segmentCount * this.segmentSize;
      this.randomGemerators = new IRandomNumberBatchGenerator[this.segmentCount];
      XorshiftRandomBatchGenerator randomBatchGenerator = new XorshiftRandomBatchGenerator((ulong) Environment.TickCount);
      ulong[] buffer = new ulong[this.segmentCount];
      randomBatchGenerator.NextBatch(buffer, 0, this.segmentCount);
      for (int index = 0; index < this.segmentCount; ++index)
      {
        Func<ulong, IRandomNumberBatchGenerator> func = (Func<ulong, IRandomNumberBatchGenerator>) (seed => (IRandomNumberBatchGenerator) new XorshiftRandomBatchGenerator(seed));
        IRandomNumberBatchGenerator numberBatchGenerator = randomGeneratorFactory == null ? func(buffer[index]) : randomGeneratorFactory(buffer[index]) ?? func(buffer[index]);
        this.randomGemerators[index] = numberBatchGenerator;
      }
      this.randomNumbers = new ulong[count];
      randomBatchGenerator.NextBatch(this.randomNumbers, 0, count);
    }

    public ulong Next()
    {
      int newIndex = Interlocked.Increment(ref this.index);
      if ((newIndex & this.randomIndexWithinSegmentMask) == 0)
        this.RegenerateSegment(newIndex);
      return this.randomNumbers[newIndex & this.randomArrayIndexMask];
    }

    private void RegenerateSegment(int newIndex)
    {
      int index = (newIndex & this.segmentIndexMask) != 0 ? ((newIndex & this.segmentIndexMask) >> this.bitsToStoreRandomIndexWithinSegment) - 1 : this.segmentCount - 1;
      this.randomGemerators[index].NextBatch(this.randomNumbers, index * this.segmentSize, this.segmentSize);
    }
  }
}
