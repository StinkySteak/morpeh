#if UNITY_EDITOR
#define MORPEH_DEBUG
#endif
#if !MORPEH_DEBUG
#define MORPEH_DEBUG_DISABLED
#endif

namespace Scellecs.Morpeh.Collections {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Unity.IL2CPP.CompilerServices;

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed unsafe class IntFastList : IEnumerable<int> {
        public int length;
        public int capacity;

        public PinnedArray<int> data;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntFastList() {
            this.capacity = 4;
            this.data     = new PinnedArray<int>(this.capacity);
            this.length   = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntFastList(int capacity) {
            this.capacity = HashHelpers.GetCapacity(capacity);
            this.data     = new PinnedArray<int>(this.capacity);
            this.length   = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntFastList(IntFastList other) {
            this.capacity = other.capacity;
            this.data     = new PinnedArray<int>(this.capacity);
            this.length   = other.length;
            Array.Copy(other.data.data, 0, this.data.data, 0, this.length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() {
            Enumerator e;
            e.intFastList = this;
            e.current     = default;
            e.index       = 0;
            return e;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public struct ResultSwap {
            public int oldIndex;
            public int newIndex;
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public struct Enumerator : IEnumerator<int> {
            public IntFastList intFastList;

            public int current;
            public int index;

            public bool MoveNext() {
                if (this.index >= this.intFastList.length) {
                    return false;
                }

                this.current = this.intFastList.data.ptr[this.index++];

                return true;
            }

            public void Reset() {
                this.index   = 0;
                this.current = default;
            }

            public int         Current => this.current;
            object IEnumerator.Current => this.current;

            public void Dispose() {
            }
        }
    }
}