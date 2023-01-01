import { useEffect, useState } from "react";
import styles from "./MessageBox.module.css";

export const MessageBox = ({ message, good, disposeMessageBox, index}) => {
    const [show, setShow] = useState(true);

    const divStyle = {
        top: `${index * 5 + 7}rem`,
    };
    
    const disposeMessageBoxSettings = () => {
        setTimeout(() => {
            disposeMessageBox();
        }, 2500);
    };
    
    useEffect(() => {
        disposeMessageBoxSettings();
        // eslint-disable-next-line
    }, []);

    return (
        show ?
            <div 
                className={good ? styles.messageBoxGreen : styles.messageBoxRed}
                style={divStyle}>
                <h5 className={styles.message}>{message}</h5>
            </div>
        :
        <></>
    );
};
