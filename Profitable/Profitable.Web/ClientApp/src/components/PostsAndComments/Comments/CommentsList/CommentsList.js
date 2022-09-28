import { CommentWidget } from "../CommentWidget/CommentWidget";

import styles from './CommentsList.module.css';
import { useEffect } from "react";

export const CommentsList = ({postId = null, userId = null, comments}) => {

    useEffect(() => {
        if(postId) {
           
        } else if(userId) {
            
        }
    }, [postId, userId]);

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