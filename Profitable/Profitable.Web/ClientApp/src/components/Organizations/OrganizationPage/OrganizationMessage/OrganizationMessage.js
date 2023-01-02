import styles from "./OrganizationMessage.module.css";

export const OrganizationMessage = ({message}) => {
    return (
        <div className={styles.messageContainer}>
            <div className={styles.infoContainer}>
                <h5 className={styles.senderContainer}>{message.sender}</h5>
                <h6 className={styles.dateContainer}>{message.sentOn}</h6>
            </div>
            <div className={styles.contentContainer}>
                <p className={styles.message}>{message.content}</p>
            </div>
        </div>
    );
}