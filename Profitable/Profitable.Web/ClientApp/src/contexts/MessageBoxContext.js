import { createContext, useState } from "react";

import { MessageBox } from "../components/Common/MessageBox/MessageBox";

export const MessageBoxContext = createContext();

export const MessageBoxContextProvider = ({ children }) => {
    const messageBoxInitialState = {
        message: "",
        good: false,
        show: false,
    };

    const [messageBox, setMessageBox] = useState([{ ...messageBoxInitialState }]);

    const setMessageBoxSettings = (message, good) => {
        setMessageBox({
            message: message,
            good,
            show: true,
        });
    };

    const disposeMessageBoxSettings = () => {
        setTimeout(() => {
            setMessageBox({ ...messageBoxInitialState });
        }, 5000);
    };
    return (
        <MessageBoxContext.Provider value={{ setMessageBoxSettings }}>
            {children}
            {messageBox.show ? (
                <MessageBox
                    message={messageBox.message}
                    good={messageBox.good}
                    disposeMessageBoxSettings={disposeMessageBoxSettings}
                />
            ) : (
                ""
            )}
        </MessageBoxContext.Provider>
    );
};
