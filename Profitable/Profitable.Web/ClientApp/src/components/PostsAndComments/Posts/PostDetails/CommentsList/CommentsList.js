import { CommentWidget } from "./CommentWidget/CommentWidget";

import styles from './CommentsList.module.css';
import { useEffect, useState } from "react";
import { getCommentsByPostId } from "../../../../../services/comments/commentsService";

export const CommentsList = ({postId = null, userId = null}) => {

    const [comments, setComments] = useState([]);

    useEffect(() => {
        if(postId) {
            getCommentsByPostId(postId)
                .then(commentsFromAPI => {
                    console.log(commentsFromAPI)
                    setComments(commentsFromAPI)
                });

        } else if(userId) {

        }

    }, [postId, userId]);

    console.log(comments);

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