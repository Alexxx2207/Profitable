import { useNavigate } from 'react-router-dom';
import styles from './CommentWidget.module.css';

export const CommentWidget = ({comment}) => {

    const navigate = useNavigate();

    const profileOpen = () => {
        navigate(`/users/${comment.authorEmail}`);
    }

    return(
        <div className={styles.commentWidget}>
            <header className={styles.commentHeader}>
                <h3 className={styles.commentAuthorName} onClick={() => profileOpen()}>{comment.authorName}</h3>
                <h6 className={styles.commentAuthorEmail} onClick={() => profileOpen()}>({comment.authorEmail})</h6>
            </header>
            <p className={styles.commentContent}>{comment.content}</p>
        </div>
    );
}