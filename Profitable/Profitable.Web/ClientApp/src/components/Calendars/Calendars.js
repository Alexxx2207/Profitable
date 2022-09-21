import { EconomicCalendarPart } from "../Calendars/EconomicCalendar/EconomicCalendar";

import styles from './Calendars.module.css';

export const Calendars = () => {
    return (
        <div className={styles.calendarsPageContainer}>
            <EconomicCalendarPart />
        </div>
    );
};