import { createContext, useState } from "react";

import { MessageBox } from "../components/Common/MessageBox/MessageBox";

export const MessageBoxContext = createContext();

export const MessageBoxContextProvider = ({ children }) => {
    const messageBoxInitialState = {
        messages: [],
    };

    const [messageBox, setMessageBox] = useState({ ...messageBoxInitialState });

    const setMessageBoxSettings = (message, good) => {
        setMessageBox(state => {
            state.messages.unshift({message, good});
            return {
                ...messageBox,
                messages: state.messages
            }
        });
    };

    const disposeMessageBox = () => {
        setMessageBox(state => {
            state.messages.shift();
            return {
                ...messageBox,
                messages: state.messages
            }
        });
    }

    return (
        <MessageBoxContext.Provider value={{ setMessageBoxSettings }}>
            {children}
            {
            messageBox.messages.map((message, index) => 
                 <MessageBox
                    key={messageBox.messages.length - index}
                    message={message.message}
                    good={message.good}
                    index={index}
                    disposeMessageBox={disposeMessageBox}
                />
            )}
           
        </MessageBoxContext.Provider>
    );
};
