import classnames from "classnames";
import { useEffect, useState, useContext, useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import { TimeContext } from "../../../../contexts/TimeContext";
import { PostsLikeWidget } from "../PostsLikeWidget/PostsLikeWidget";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faTrash,
    faPenToSquare,
    faPlusCircle,
    faCircleMinus,
} from "@fortawesome/free-solid-svg-icons";

import { deletePost, loadParticularPost } from "../../../../services/posts/postsService";
import { createImgURL, createAuthorImgURL } from "../../../../services/common/imageService";
import { getUserDataByJWT, getUserEmailFromJWT } from "../../../../services/users/usersService";

import {
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    MISSING_POST_GUID_ERROR_PAGE_PATH,
    COMMENTS_LIST_IN_POST_PAGE_COUNT,
} from "../../../../common/config";

import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { CommentsList } from "../../Comments/CommentsList/CommentsList";
import { CreateComment } from "../../Comments/CreateComment/CreateComment";
import { getCommentsByPostId } from "../../../../services/comments/commentsService";

import { convertFullDateTime } from "../../../../utils/Formatters/timeFormatter";

import { ShowMoreButton } from "../../../Common/ShowMoreButton/ShowMoreButton";
import { GoToTop } from "../../../Common/GoToTop/GoToTop";

import styles from "./PostDetails.module.css";

export const PostDetails = () => {
    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);

    const { timeOffset } = useContext(TimeContext);

    const { postId } = useParams();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useState({
        post: {
            guid: "",
            title: "",
            content: "",
            author: "",
            authorEmail: "",
            authorImage: "",
            postedOn: "",
            postImage: "",
            isLikedByTheUser: false,
            likes: [],
            comments: [],
        },
        showCreateCommentWidget: false,
        page: 0,
        showShowMore: true,
    });

    const [userEmail, setUserEmail] = useState("");

    const loadComments = useCallback(
        (page, pageCount) => {
            getCommentsByPostId(postId, page, pageCount).then((commentsFromAPI) => {
                if (commentsFromAPI.length > 0) {
                    setState((oldState) => ({
                        ...oldState,
                        page: oldState.page + 1,
                        post: {
                            ...oldState.post,
                            comments: [...oldState.post.comments, ...commentsFromAPI],
                        },
                    }));
                } else {
                    setMessageBoxSettings("There are no more comments", false);
                    setState((state) => ({
                        ...state,
                        showShowMore: false,
                    }));
                }
            });
        },
        [postId, timeOffset]
    );

    const loadFirstComments = useCallback(() => {
        getCommentsByPostId(postId, 0, COMMENTS_LIST_IN_POST_PAGE_COUNT).then((commentsFromAPI) => {
            if (commentsFromAPI.length > 0) {
                setState((state) => ({
                    ...state,
                    post: {
                        ...state.post,
                        comments: [...commentsFromAPI],
                    },
                }));
            } else {
                setState((state) => ({
                    ...state,
                    showShowMore: false,
                }));
            }
        });
    }, [postId, timeOffset]);

    const handleShowMoreCommentsClick = useCallback(
        (e) => {
            e.preventDefault();
            setState((oldState) => ({
                ...oldState,
            }));
            loadComments(state.page + 1, COMMENTS_LIST_IN_POST_PAGE_COUNT);
        },
        [loadComments, state.page]
    );

    useEffect(() => {
        getUserEmailFromJWT(JWT).then((result) => {
            setUserEmail(result);
        });
    }, [JWT]);

    useEffect(() => {
        loadParticularPost(postId, userEmail)
            .then((result) => {
                var offsetedTime = new Date(
                    new Date(result.postedOn).getTime() - timeOffset * 60000
                );
                setState((state) => ({
                    ...state,
                    post: {
                        ...result,
                        postedOn: convertFullDateTime(offsetedTime),
                    },
                }));
                loadFirstComments();
            })
            .catch((err) => navigate(`${MISSING_POST_GUID_ERROR_PAGE_PATH}`));
    }, [postId, navigate, loadFirstComments, userEmail]);

    const goToEditPageHandler = (e) => {
        getUserDataByJWT(JWT)
            .then((result) => {
                navigate(`/posts/${postId}/edit`);
            })
            .catch((err) => {
                if (JWT && err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    removeAuth();
                } else {
                    setMessageBoxSettings("You should login before editing a post!", false);
                }
                navigate("/login");
            });
    };

    const deletePostClickHandler = () => {
        deletePost(JWT, postId)
            .then((result) => {
                setMessageBoxSettings("The post was deleted successfully!", true);
                navigate("/posts");
            })
            .catch((err) => {
                if (JWT && err.message === "Should auth first") {
                    setMessageBoxSettings(
                        JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE + " The post was not deleted!",
                        false
                    );
                    removeAuth();
                } else {
                    setMessageBoxSettings("You should login before editing a post!", false);
                }
                navigate("/login");
            });
    };

    const clickUserProfileHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();

        navigate(`/users/${state.post.authorEmail}`);
    };

    const handleAddCommentButton = useCallback(() => {
        setState((state) => ({
            ...state,
            showCreateCommentWidget: !state.showCreateCommentWidget,
            showShowMore: true,
        }));
    }, []);

    return (
        <div className={styles.state}>
            <div className={styles.post}>
                <div className={styles.buttonContainer}>
                    {state.post.authorEmail && state.post.authorEmail === userEmail ? (
                        <div className={styles.ownerButtonSection}>
                            <button
                                className={classnames(styles.button, styles.editButton)}
                                onClick={goToEditPageHandler}
                            >
                                <FontAwesomeIcon className={styles.iconEdit} icon={faPenToSquare} />
                                <div className={styles.backText}>Edit</div>
                            </button>
                            <button
                                className={classnames(styles.button, styles.deleteButton)}
                                onClick={deletePostClickHandler}
                            >
                                <FontAwesomeIcon className={styles.iconDelete} icon={faTrash} />
                                <div className={styles.backText}>Delete</div>
                            </button>
                        </div>
                    ) : (
                        ""
                    )}
                </div>
                <div className={styles.postContent}>
                    <div className={styles.text}>
                        <h1 className={styles.title}>{state.post.title}</h1>
                        {state.post.postImage ? (
                            <img
                                className={styles.postImage}
                                src={createImgURL(state.post.postImage)}
                                alt=""
                            />
                        ) : (
                            ""
                        )}
                        <div className={styles.content}>
                            {state.post.content.split("\\n").map((paragraph, index) => (
                                <p key={index}>
                                    {paragraph}
                                    <br />
                                </p>
                            ))}
                        </div>
                        <div className={styles.postsLikeWidgetContainer}>
                            <PostsLikeWidget style={styles.postsLikeWidget} post={state.post} />
                        </div>
                    </div>
                    <div className={styles.information}>
                        <div className={styles.author}>
                            <img
                                onClick={clickUserProfileHandler}
                                className={styles.authorImage}
                                src={createAuthorImgURL(state.post.authorImage)}
                                alt=""
                            />
                            <div onClick={clickUserProfileHandler} className={styles.authorName}>
                                {state.post.author}
                            </div>
                        </div>
                        <div className={styles.postedOn}>{state.post.postedOn}</div>
                    </div>
                </div>
            </div>
            <section className={styles.commentsSection}>
                <div className={styles.commentsHeaderContainer}>
                    <h1 className={styles.commentsHeader}>Comments</h1>
                    {JWT ? (
                        <div className={styles.addCommentButtonContainer}>
                            <button
                                className={styles.addCommentButton}
                                onClick={handleAddCommentButton}
                            >
                                {state.showCreateCommentWidget ? (
                                    <>
                                        <FontAwesomeIcon
                                            className={styles.iconAddComment}
                                            icon={faCircleMinus}
                                        />
                                        Hide Adding Panel
                                    </>
                                ) : (
                                    <>
                                        <FontAwesomeIcon
                                            className={styles.iconAddComment}
                                            icon={faPlusCircle}
                                        />
                                        Add Comment
                                    </>
                                )}
                            </button>
                        </div>
                    ) : (
                        ""
                    )}
                </div>
                {state.showCreateCommentWidget ? (
                    <CreateComment
                        postId={postId}
                        loadFirstComments={loadFirstComments}
                        handleAddCommentButton={handleAddCommentButton}
                    />
                ) : (
                    <></>
                )}
                <div className={styles.commentsListContainer}>
                    <CommentsList postId={postId} comments={state.post.comments} />
                </div>
            </section>

            <ShowMoreButton
                entity={"Comments"}
                showShowMore={state.showShowMore}
                handler={handleShowMoreCommentsClick} />

            <GoToTop />
        </div>
    );
};
