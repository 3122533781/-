using System;

public class RatingModel
{
    public Action<int> OnRatingStarChanged = delegate { };

    public int ShowRatingNum { get; set; }
    public PersistenceData<int> LastShowTimestamp { get; set; }

    public int GetToLastShowTime()
    {
        int now = DataFormater.ConvertDateTimeToTimeStamp(DateTime.Now);

        return now - LastShowTimestamp.Value;
    }

    public int CurrentRatingStar
    {
        get => _curRatingStar;
        set
        {
            _curRatingStar = value;
            OnRatingStarChanged?.Invoke(_curRatingStar);
        }
    }

    public RatingModel()
    {
        ShowRatingNum = 0;
        LastShowTimestamp = new PersistenceData<int>("RatingModel_LastShowTimestamp", 0);
    }

    private int _curRatingStar = 0;
}