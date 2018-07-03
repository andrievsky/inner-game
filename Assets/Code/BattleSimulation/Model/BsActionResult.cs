namespace Code.BattleSimulation.Model
{
    // Common action result data
    public class BsActionResult
    {
        // Ok = true if action has been applied
        public bool Ok;

        // Value could be used to modify and apply process results
        public int Value;

        // Reason contains additional info
        public string Reason = "";

        public void Reset()
        {
            Ok = false;
            Value = 0;
            Reason = "";
        }
    }
}