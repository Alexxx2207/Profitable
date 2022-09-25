import classnames from "classnames";
import { useEffect, useState, useContext } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { AuthContext } from '../../../../contexts/AuthContext';
import { deletePost, loadParticularPost } from '../../../../services/posts/postsService';
import { PostsLikeWidget } from '../PostsLikeWidget/PostsLikeWidget';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowCircleLeft, faTrash, faPenToSquare } from '@fortawesome/free-solid-svg-icons';

import { createImgURL, createAuthorImgURL } from '../../../../services/common/imageService';
import { getUserDataByJWT, getUserEmailFromJWT } from '../../../../services/users/usersService';

import { JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, MISSING_POST_GUID_ERROR_PAGE_PATH } from '../../../../common/config';

import styles from './PostDetails.module.css';
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { CommentsList } from "./CommentsList/CommentsList";

export const PostDetails = () => {

    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);

    const { postId } = useParams();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [post, setPost] = useState({
        guid: '',
        title: '',
        content: '',
        author: '',
        authorEmail: '',
        authorImage: '',
        postedOn: '',
        postImage: '',
        likes: [],
        comments: [],
    });

    const [userEmail, setUserEmail] = useState('');

    useEffect(() => {
        loadParticularPost(postId)
            .then(result => setPost(result))
            .catch(err => navigate(`${MISSING_POST_GUID_ERROR_PAGE_PATH}`))
    }, [postId, navigate]);

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then(email =>
                setUserEmail(state => email)
            )
            .catch(err => err)
    }, [JWT]);

    const goBackHandler = (e) => {
        navigate('/posts');
    }

    const goToEditPageHandler = (e) => {
        getUserDataByJWT(JWT)
            .then(result => {
                navigate(`/posts/${postId}/edit`);
            })
            .catch(err => {
                if (JWT && err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    removeAuth();
                } else {
                    setMessageBoxSettings('You should login before editing a post!', false);
                }
                navigate('/login');
            });
    };

    const deletePostClickHandler = () => {
        deletePost(JWT, postId)
            .then(result => {
                setMessageBoxSettings('The post was deleted successfully!', true);
                navigate('/posts')
            })
            .catch(err => {
                if (JWT && err.message === 'Should auth first') {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE + ' The post was not deleted!', false);
                    removeAuth();
                } else {
                    setMessageBoxSettings('You should login before editing a post!', false);
                }
                navigate('/login');
            })
    };

    const clickUserProfileHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();

        navigate(`/users/${post.authorEmail}`);
    };

    return (
        <div className={styles.postPage}>
            <div className={styles.post}>
                <div className={styles.buttonContainer}>
                    <button className={classnames(styles.button, styles.backButton)} onClick={goBackHandler}>
                        <FontAwesomeIcon className={styles.iconLeftArrow} icon={faArrowCircleLeft} />
                        <div className={styles.backText}>
                            Go Back
                        </div>
                    </button>
                    {post.authorEmail && post.authorEmail === userEmail ?
                        <div className={styles.ownerButtonSection}>
                            <button className={classnames(styles.button, styles.editButton)} onClick={goToEditPageHandler}>
                                <FontAwesomeIcon className={styles.iconEdit} icon={faPenToSquare} />
                                <div className={styles.backText}>
                                    Edit
                                </div>
                            </button>
                            <button className={classnames(styles.button, styles.deleteButton)} onClick={deletePostClickHandler}>
                                <FontAwesomeIcon className={styles.iconDelete} icon={faTrash} />
                                <div className={styles.backText}>
                                    Delete
                                </div>
                            </button>
                        </div>
                        :
                        ''
                    }
                </div>
                <div className={styles.postContent}>
                    <div className={styles.text}>
                        <h1 className={styles.title}>{post.title}</h1>
                        {post.postImage ?
                            <img className={styles.postImage} src={createImgURL(post.postImage)} alt="" />
                            :
                            ""
                        }
                        <div className={styles.content}>
                            {post.content.split('\\n').map((paragraph, index) =>
                                <p key={index}>{paragraph}<br /></p>
                            )}
                        </div>
                        <div className={styles.postsLikeWidgetContainer}>
                            <PostsLikeWidget style={styles.postsLikeWidget} post={post} />
                        </div>
                    </div>
                    <div className={styles.information}>
                        <div className={styles.author}>
                            <img onClick={clickUserProfileHandler} className={styles.authorImage} src={createAuthorImgURL(post.authorImage)} alt="" />
                            <div onClick={clickUserProfileHandler} className={styles.authorName}>
                                {post.author}
                            </div>
                        </div>
                        <div className={styles.postedOn}>
                            {post.postedOn}
                        </div>
                    </div>
                </div>
            </div>
            <section className={styles.commentsSection}>
                <h1 className={styles.commentsHeader}>Comments</h1>
                <CommentsList postId={postId} />
            </section>
        </div>
    );
}