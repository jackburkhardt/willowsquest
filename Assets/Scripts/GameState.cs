public enum GameState
{
     Start, // objective: talk to tutorial npcs
     CheckFirstGate, // check first gatekeeper
     FightSquirrels, // objective: fight the squirrels to get the key location
     Stump, // objective: get ket from bush, open gate
     CheckSecondGate, // objective: walk over to the second gate (realize gatekeeper is asleep)
     MeetFrog, // objective: talk to frog who may be able to help
     FindWorms, // objective: frog has you find worms hidden under crates (requires pushing them around)
     GiveWorms, // objective: give worms back to frog
     MeetCobra, // objective: go thru gate, talk to king cobra
     FightWolves, // objective: fight the wolves for the crown
     GiveCrown, // objective: give crown to king cobra
     FightBear, // objective: defeat the bear
     Finish // objective: save ur friend
}

