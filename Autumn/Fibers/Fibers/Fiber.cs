using System;

namespace Fibers
{
    public class Fiber
    {
        private Action action;
        public uint Id { get; private set; }

        public static uint PrimaryId { get; private set; }
        public bool IsPrimary { get; private set; }

        public Fiber(Action action)
        {
            InnerCreate(action);
        }

        public void Delete()
        {
            UnmanagedFiberAPI.DeleteFiber(Id);
        }

        public static void Delete(uint fiberId)
        {
            UnmanagedFiberAPI.DeleteFiber(fiberId);
        }

        public static void Switch(uint fiberId)
        {
            Console.WriteLine(string.Format("Fiber [{0}] Switch", fiberId));

            UnmanagedFiberAPI.SwitchToFiber(fiberId);
        }

        private void InnerCreate(Action action)
        {
            this.action = action;

            if (PrimaryId == 0)
            {
                PrimaryId = UnmanagedFiberAPI.ConvertThreadToFiber(0);
                IsPrimary = true;
            }

            UnmanagedFiberAPI.LPFIBER_START_ROUTINE lpFiber = FiberRunnerProc;
            Id = UnmanagedFiberAPI.CreateFiber(100500, lpFiber, 0);
        }

        private uint FiberRunnerProc(uint lpParam)
        {
            uint status = 0;
        
            try
            {
                action();
            }
            catch (Exception)
            {
                status = 1;
                throw;
            }
            finally
            {
                if (status == 1)
                    UnmanagedFiberAPI.DeleteFiber((uint)Id);
            }
        
            return status;
        }
    }
}
