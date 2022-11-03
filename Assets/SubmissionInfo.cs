
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-fantastic-four";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "Fantastic Four";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        new TeamMember("Qianwen Wu", "qianwenw1@student.unimelb.edu.au"),
        new TeamMember("Bohan Liang", "lianbl@student.unimelb.edu.au"),
        new TeamMember("Xinxin Guo", "xdguo@student.unimelb.edu.au"),
        new TeamMember("Cheng Yin Loh", "chengyinl@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "The Lost Ghost";

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = 
$@"{GameName} is an super casual arcade game where you help the character find 
their way home (the tomb). Almost all objects in the game has their position
randomly generated, so there won't be similar layouts. There is no losing in 
this game, and you can take your time admiring the beauty of the map. Though, 
there is a timer built-in if you ever want to speed-run it. :)";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://www.youtube.com/watch?v=XMnCgaJzN5E&ab";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
