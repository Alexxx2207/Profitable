import { CommentWidget } from "../CommentWidget/CommentWidget";

import styles from './CommentsList.module.css';

export const CommentsList = ({comments}) => {
    return (
        <div className={styles.commentsListContainer}>
            {comments.length > 0 ?
                comments.map((comment, index) => <CommentWidget key={index} comment={comment} />)
            :
                <h3 className={styles.noCommentsYetMessage}>No Comments Yet</h3>
            }
        </div>
    );
}