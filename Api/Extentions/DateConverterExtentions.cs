namespace Api.Extentions
{
    public static class DateConverterExtentions
    {
        public static long DateToNumber(DateTime date)
        {
            DateTimeOffset dto = new DateTimeOffset(Convert.ToDateTime(date));
            long numberDate = dto.ToUnixTimeSeconds();
            return numberDate;
        }
    }
}