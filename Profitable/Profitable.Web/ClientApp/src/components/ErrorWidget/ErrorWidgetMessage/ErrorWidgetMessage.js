import styles from './ErrorWidgetMessage.module.css';

export const ErrorWidgetMessage = ({message}) => {
    console.log(message.split('\\n'));
    if(message.includes('\\n')) {
        return <div className={styles.messageParagraphsContainer}>
            {
                message.split('\\n').map( (paragraph, index) =>
                <p className={styles.messageParagraph} key={index}>{paragraph}</p>)
            }
        </div>;
    } else {
        return message;
    }
}