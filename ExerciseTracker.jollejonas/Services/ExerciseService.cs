using ExerciseTracker.jollejonas.Repositories;
using ExerciseTracker.jollejonas.Models;
using ExerciseTracker.jollejonas.UserInput;
public class ExerciseService
    {
    private readonly IExerciseRepository _exercieseRepository;
    private readonly IUserInput _userInput;

    public ExerciseService(IExerciseRepository exerciseRepository, IUserInput userInput)
    {
        _exercieseRepository = exerciseRepository;
        _userInput = userInput;
    }

    public void AddExercise()
    {
        var exercise = new Exercise();
        Console.WriteLine("Start time: ");
        exercise.DateStart = _userInput.GetDateTime();
        Console.WriteLine("End time: ");
        exercise.DateEnd = _userInput.GetDateTime();

        exercise.Duration = CalculateDuration(exercise.DateStart, exercise.DateEnd);

        exercise.Comments = _userInput.GetExerciseComments();

        _exercieseRepository.AddExercise(exercise);
    }

    public void DeleteExercise()
    {
        Exercise selectedExercise = _userInput.GetExercise(_exercieseRepository.GetAllExercises());
        _exercieseRepository.DeleteExercise(selectedExercise);
    }

    public List<Exercise> GetAllExercises()
    {
        return _exercieseRepository.GetAllExercises();
    }

    public void UpdateExercise()
    {
        Exercise selectedExercise = _userInput.GetExercise(_exercieseRepository.GetAllExercises());

        if (selectedExercise == null)
        {
            Console.WriteLine("Exercise not found.");
            return;
        }

        var oldExercise = _exercieseRepository.GetExerciseById(selectedExercise.Id);

        Exercise updatedExercise = new Exercise();

        if (_userInput.GetConfirmation("Do you want to update the start time? (y/n)"))
        {
            Console.WriteLine("Start time: ");
            updatedExercise.DateStart = _userInput.GetDateTime();
        }
        else
        {
            updatedExercise.DateStart = oldExercise.DateStart;
        }
        if(_userInput.GetConfirmation("Do you want to update the end time? (y/n)"))
        {
            Console.WriteLine("End time: ");
            updatedExercise.DateEnd = _userInput.GetDateTime();
            updatedExercise.Duration = CalculateDuration(updatedExercise.DateStart, updatedExercise.DateEnd);
        }
        else
        {
            updatedExercise.DateEnd = oldExercise.DateEnd;
            updatedExercise.Duration = CalculateDuration(updatedExercise.DateStart, updatedExercise.DateEnd);
        }

        if (_userInput.GetConfirmation("Do you want to update the comments? (y/n)"))
        {
            updatedExercise.Comments = _userInput.GetExerciseComments();
        }
        else
        {
            updatedExercise.Comments = oldExercise.Comments;
        }

        _exercieseRepository.UpdateExercise(updatedExercise);
    }

    public TimeSpan CalculateDuration(DateTime dateStart, DateTime dateEnd)
    {
        return dateEnd - dateStart;
    }
}
