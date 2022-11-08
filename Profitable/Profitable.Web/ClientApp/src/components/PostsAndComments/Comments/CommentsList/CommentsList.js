import { useContext, useEffect, useState } from "react";
import { TimeContext } from "../../../../contexts/TimeContext";
import { convertFullDateTime } from "../../../../utils/Formatters/timeFormatter";
import { CommentWidget } from "../CommentWidget/CommentWidget";

import styles from "./CommentsList.module.css";

export const CommentsList = ({ comments }) => {
    var { timeOffset } = useContext(TimeContext);

    var [state, setState] = useState(comments);
    useEffect(() => {
        var commentsWithOffsetedTime = [
            ...comments.map((comment) => ({
                ...comment,
                postedOn: convertFullDateTime(
                    new Date(new Date(comment.postedOn).getTime() - timeOffset * 60000)
                ),
            })),
        ];
        setState((oldState) => [...commentsWithOffsetedTime]);
    }, [comments]);

    return (
        <div className={styles.commentsListContainer}>
            {comments.length > 0 ? (
                state.map((comment, index) => (
                    <CommentWidget key={index} comment={comment} deleteFromList={null} />
                ))
            ) : (
                <h3 className={styles.noCommentsYetMessage}>No Comments Yet</h3>
            )}
        </div>
    );
};
