import classnames from "classnames";
import { useEffect, useState, useContext, useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
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

import { GoToTop } from "../../../Common/GoToTop/GoToTop";

import styles from "./PostDetails.module.css";

export const PostDetails = () => {
    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);

    const { postId } = useParams();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [postPage, setPostPage] = useState({
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
    });

    const [userEmail, setUserEmail] = useState("");

    let page = 0;

    const loadComments = useCallback(
        (page, pageCount) => {
            getCommentsByPostId(postId, page, pageCount).then((commentsFromAPI) => {
                setPostPage((state) => ({
                    ...state,
                    post: {
                        ...state.post,
                        comments: [...state.post.comments, ...commentsFromAPI],
                    },
                }));
            });
        },
        [postId]
    );

    const loadFirstComments = useCallback(() => {
        getCommentsByPostId(postId, 0, COMMENTS_LIST_IN_POST_PAGE_COUNT).then((commentsFromAPI) => {
            if (commentsFromAPI.length > 0) {
                setPostPage((state) => ({
                    ...state,
                    post: {
                        ...state.post,
                        comments: [...commentsFromAPI],
                    },
                }));
            }
        });
    }, [postId]);

    const handleScroll = useCallback(
        (e) => {
            const scrollHeight = e.target.documentElement.scrollHeight;
            const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

            if (currentHeight >= scrollHeight - 300 || currentHeight === scrollHeight) {
                page++;
                loadComments(page, COMMENTS_LIST_IN_POST_PAGE_COUNT);
            }
        },
        [loadComments, page]
    );

    useEffect(() => {
        getUserEmailFromJWT(JWT).then((result) => {
            setUserEmail(result);
        });
    }, [JWT]);

    useEffect(() => {
        window.addEventListener("scroll", handleScroll);

        return () => window.removeEventListener("scroll", handleScroll);
    }, [handleScroll]);

    useEffect(() => {
        loadParticularPost(postId, userEmail)
            .then((result) => {
                setPostPage((state) => ({
                    ...state,
                    post: { ...result },
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

        navigate(`/users/${postPage.post.authorEmail}`);
    };

    const handleAddCommentButton = useCallback(() => {
        setPostPage((state) => ({
            ...state,
            showCreateCommentWidget: !state.showCreateCommentWidget,
        }));
    }, []);

    return (
        <div className={styles.postPage}>
            <div className={styles.post}>
                <div className={styles.buttonContainer}>
                    {postPage.post.authorEmail && postPage.post.authorEmail === userEmail ? (
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
                        <h1 className={styles.title}>{postPage.post.title}</h1>
                        {postPage.post.postImage ? (
                            <img
                                className={styles.postImage}
                                src={createImgURL(postPage.post.postImage)}
                                alt=""
                            />
                        ) : (
                            ""
                        )}
                        <div className={styles.content}>
                            {postPage.post.content.split("\\n").map((paragraph, index) => (
                                <p key={index}>
                                    {paragraph}
                                    <br />
                                </p>
                            ))}
                        </div>
                        <div className={styles.postsLikeWidgetContainer}>
                            <PostsLikeWidget style={styles.postsLikeWidget} post={postPage.post} />
                        </div>
                    </div>
                    <div className={styles.information}>
                        <div className={styles.author}>
                            <img
                                onClick={clickUserProfileHandler}
                                className={styles.authorImage}
                                src={createAuthorImgURL(postPage.post.authorImage)}
                                alt=""
                            />
                            <div onClick={clickUserProfileHandler} className={styles.authorName}>
                                {postPage.post.author}
                            </div>
                        </div>
                        <div className={styles.postedOn}>Posted On: {postPage.post.postedOn}</div>
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
                                {postPage.showCreateCommentWidget ? (
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
                {postPage.showCreateCommentWidget ? (
                    <CreateComment
                        postId={postId}
                        loadFirstComments={loadFirstComments}
                        handleAddCommentButton={handleAddCommentButton}
                    />
                ) : (
                    <></>
                )}
                <div className={styles.commentsListContainer}>
                    <CommentsList postId={postId} comments={postPage.post.comments} />
                </div>
            </section>
            <GoToTop />
        </div>
    );
};
