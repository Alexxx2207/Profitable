import { createContext, useEffect, useState } from "react";

export const TimeContext = createContext();

export const TimeContextProvider = ({ children }) => {
    const [timeOffset, setTimeOffset] = useState("");

    useEffect(() => {
        const offset = new Date().getTimezoneOffset();
        setTimeOffset((state) => offset);
    }, []);

    return (
        <TimeContext.Provider value={{ timeOffset: timeOffset }}>{children}</TimeContext.Provider>
    );
};
