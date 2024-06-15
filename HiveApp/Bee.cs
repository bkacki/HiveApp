using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveApp
{
    abstract class Bee
    {
        public Bee(string beeRole) 
        {
            Job = beeRole;
        }

        public string Job { get; private set; }
        public abstract float CostPerShift { get; }

        public void WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift))
                DoJob();
        }

        protected abstract void DoJob();
    }

    internal class Queen : Bee
    {
        public const float EGGS_PER_SHIFT = 0.45F;
        public const float HONEY_PER_UNASSIGNED_WORKER = 0.5F;

        public Queen() : base("Królowa") 
        {
            AssignBee("Zbieraczka nektaru");
            AssignBee("Producentka miodu");
            AssignBee("Opiekunka jaj");
        }

        public override float CostPerShift { get { return 2.15F; } }

        protected override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;
            foreach (Bee worker in workers)
                worker.WorkTheNextShift();

            HoneyVault.ConsumeHoney(unassignedWorkers * HONEY_PER_UNASSIGNED_WORKER);
            UpdateStatusReport();
        }

        private void UpdateStatusReport()
        {
            StatusReport = $"Raport o stanie skarbca \n{HoneyVault.StatusReport}\n" +
                $"Liczba jaj: {eggs:0.0}\n" +
                $"Nieprzydzielone robotnice: {unassignedWorkers:0.0}\n" +
                $"{WorkerStatus("Zbieraczka nektaru")}\n{WorkerStatus("Producentka miodu")}\n{WorkerStatus("Opiekunka jaj")}" +
                $"\nROBOTNICE W SUMIE: {workers.Length}";
        }

        private Bee[] workers = new Bee[0];
        private float eggs = 0;
        private float unassignedWorkers = 3;

        public string StatusReport { get; private set; }

        private void AddWorker(Bee worker)
        {
            if(unassignedWorkers >= 1)
            {
                unassignedWorkers--;
                Array.Resize(ref workers, workers.Length+1);
                workers[workers.Length - 1] = worker;
            }
        }

        public void AssignBee(string job)
        {
            switch (job)
            {
                case "Zbieraczka nektaru":
                    AddWorker(new NectarCollector()); break;
                case "Producentka miodu":
                    AddWorker(new HoneyManufacturer()); break;
                case "Opiekunka jaj":
                    AddWorker(new EggCare(this)); break;
            }
        }

        public void CareForEggs(float eggsToConvert)
        {
            if (eggs >= eggsToConvert)
            {
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }

        private string WorkerStatus (string job)
        {
            int count = 0;
            foreach (Bee worker in workers)
                if (worker.Job == job) count++;
            return $"{job}: {count}";
        }
    }

    internal class HoneyManufacturer : Bee
    {
        public HoneyManufacturer() : base("Producentka miodu") { }

        public override float CostPerShift { get { return 1.7F;  } }

        public const float NECTAR_PROCESSED_PER_SHIFT = 33.15F;

        protected override void DoJob()
        {
            HoneyVault.ConvertNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
        }
    }

    internal class NectarCollector : Bee
    {
        public NectarCollector() : base("Zbieraczka nektaru") { }

        public override float CostPerShift { get { return 1.95F; } }

        public const float NECTAR_COLLECTED_PER_SHIFT = 33.25F;

        protected override void DoJob()
        {
            HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT);
        }
    }

    internal class EggCare : Bee
    {
        public const float CARE_PROGRESS_PER_SHIFT = 0.15F;

        private Queen queen;

        public EggCare(Queen queen) : base("Opiekunka jaj") 
        {
            this.queen = queen;
        }

        public override float CostPerShift { get { return 1.35F;  } }

        protected override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
    }
}
