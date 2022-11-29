export const getLastDayOFWeekDate = (dayOfWeek) => {
    var currentDate = new Date();

    while (currentDate.getDay() !== dayOfWeek) {
        currentDate.setDate(currentDate.getDate() - 1);
    }
    return currentDate;
};
