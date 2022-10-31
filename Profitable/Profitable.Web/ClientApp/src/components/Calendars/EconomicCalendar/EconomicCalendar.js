import { useEffect, useState } from "react";
import { EconomicCalendar } from "react-ts-tradingview-widgets";

import styles from "./EconomicCalendar.module.css";

export const EconomicCalendarPart = () => {
    const [ignored, forceUpdate] = useState("");

    useEffect(() => {
        document.querySelector("#tradingview_widget_wrapper").children[0].style.width = "100%";
        document.querySelector("#tradingview_widget_wrapper").children[0].style.height = "100%";
        document.querySelector("#tradingview_widget_wrapper").style.width = "100%";
        document.querySelector("#tradingview_widget_wrapper").style.height = "85vh";
        window.addEventListener("resize", forceUpdate);
    }, []);

    return (
        <div className={styles.calendarsPageContainer}>
            <div className={styles.economicCalendarContainer}>
                <EconomicCalendar colorTheme="dark" autosize></EconomicCalendar>
            </div>
        </div>
    );
};
