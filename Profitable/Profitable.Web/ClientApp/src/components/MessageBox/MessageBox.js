import { useEffect } from 'react';
import styles from './MessageBox.module.css';

export const MessageBox = ({message, good, disposeMessageBoxSettings}) => {

    useEffect(() => {
        disposeMessageBoxSettings();
        // eslint-disable-next-line
    }, []);

    return (
        <div className={good ? styles.messageBoxGreen : styles.messageBoxRed}>
            <h5 className={styles.message}>{message}</h5>
        </div>
    );
}