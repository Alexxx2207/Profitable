import { useEffect } from 'react';
import styles from './MessageBox.module.css';

export const MessageBox = ({message, good, disposeMessageBoxSettings}) => {

    useEffect(() => {
        disposeMessageBoxSettings();
    }, []);

    return (
        <div className={good ? styles.messageBoxGreen : styles.messageBoxRed}>
            <h3 className={styles.message}>{message}</h3>
        </div>
    );
}