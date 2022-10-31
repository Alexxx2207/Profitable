import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash, faPenToSquare, faX, faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import styles from "./CommentWidget.module.css";
import { deleteComment, editComment } from "../../../../services/comments/commentsService";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { AuthContext } from "../../../../contexts/AuthContext";
import { getUserDataByJWT } from "../../../../services/users/usersService";
import { JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE } from "../../../../common/config";

export const CommentWidget = ({ comment }) => {
    const { setMessageBoxSettings } = useContext(MessageBoxContext);
    const { JWT } = useContext(AuthContext);

    const [state, setState] = useState({
        accountEmail: "",
        hideCommentWhenDeleted: false,
        editState: false,
        editContent: comment.content,
    });

    const navigate = useNavigate();

    useEffect(() => {
        getUserDataByJWT(JWT).then((result) => {
            setState((state) => ({
                ...state,
                accountEmail: result.email,
            }));
        });
    }, [JWT]);

    const profileOpen = () => {
        navigate(`/users/${comment.authorEmail}`);
    };

    const handleOnChangeEdit = (e) => {
        setState((state) => ({
            ...state,
            editContent: e.target.value,
        }));
    };

    const handleOnClickEdit = () => {
        setState((state) => ({
            ...state,
            editState: !state.editState,
            editContent: comment.content,
        }));
    };

    const handleOnClickEditSend = () => {
        editComment(JWT, comment.guid, state.editContent)
            .then((response) => {
                setMessageBoxSettings("Comment was edited successfully!", true);
                comment.content = state.editContent;
                setState((state) => ({
                    ...state,
                    editState: !state.editState,
                }));
            })
            .catch((err) => {
                if (err.message === "401") {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    navigate("/login");
                } else if (err.message === "400") {
                    setMessageBoxSettings("Comment was not edited successfully!", false);
                }
            });
    };

    const handleOnClickDelete = () => {
        deleteComment(JWT, comment.guid)
            .then((response) => {
                setMessageBoxSettings("Comment was deleted successfully!", true);
                setState((state) => ({
                    ...state,
                    hideCommentWhenDeleted: true,
                }));
            })
            .catch((err) => {
                if (err.message === "401") {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    navigate("/login");
                } else if (err.message === "400") {
                    setMessageBoxSettings("Comment was not deleted successfully!", false);
                }
            });
    };

    return (
        <>
            {!state.hideCommentWhenDeleted ? (
                <div className={styles.commentWidget}>
                    <header className={styles.commentHeader}>
                        <h3 className={styles.commentAuthorName} onClick={() => profileOpen()}>
                            {comment.authorName}
                        </h3>
                        <h6 className={styles.commentAuthorEmail} onClick={() => profileOpen()}>
                            ({comment.authorEmail})
                        </h6>
                    </header>
                    {state.editState ? (
                        <>
                            <textarea
                                className={styles.commentTextArea}
                                onChange={handleOnChangeEdit}
                                value={state.editContent}
                                rows="10"
                            >
                                {comment.content}
                            </textarea>
                            <div className={styles.buttonSendContainer}>
                                <button
                                    className={styles.buttonSend}
                                    onClick={handleOnClickEditSend}
                                >
                                    <FontAwesomeIcon icon={faPaperPlane} />
                                    Send
                                </button>
                            </div>
                        </>
                    ) : (
                        <p className={styles.commentContent}>{comment.content}</p>
                    )}
                    <footer className={styles.commentFooter}>
                        <h6 className={styles.postedOnContainer}>{comment.postedOn}</h6>

                        {state.accountEmail === comment.authorEmail ? (
                            <section className={styles.authorSection}>
                                <button
                                    className={styles.editCommentButton}
                                    onClick={handleOnClickEdit}
                                >
                                    {state.editState ? (
                                        <>
                                            <FontAwesomeIcon icon={faX} />
                                            Dismiss
                                        </>
                                    ) : (
                                        <>
                                            <FontAwesomeIcon icon={faPenToSquare} />
                                            Edit
                                        </>
                                    )}
                                </button>
                                <button
                                    className={styles.removeCommentButton}
                                    onClick={handleOnClickDelete}
                                >
                                    <FontAwesomeIcon icon={faTrash} />
                                    Delete
                                </button>
                            </section>
                        ) : (
                            ""
                        )}
                    </footer>
                </div>
            ) : (
                ""
            )}
        </>
    );
};
