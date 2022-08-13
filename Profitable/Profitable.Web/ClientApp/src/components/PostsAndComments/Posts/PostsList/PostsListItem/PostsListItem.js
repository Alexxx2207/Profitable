import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faComments } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';
import { createAuthorImgURL } from '../../../../../services/common/imageService';
import { PostsLikeWidget } from '../../PostsLikeWidget/PostsLikeWidget';
import styles from './PostsListItem.module.css';

export const PostsListItem = ({post}) => {

    const navigate = useNavigate();

    const postDetailsClickHandler = () => {
        navigate(`/posts/${post.guid}`, { replace: false });
    }

    const clickUserProfileHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();

        navigate(`/users/${post.authorEmail}`);
    };

    return (
        <div className={styles.post} onClick={postDetailsClickHandler}>
            <div className={styles.header}>
                <h2 className={styles.title}>{post.title}</h2>
                <PostsLikeWidget likesCount={post.likes.length} post={post} />
            </div>

            <div className={styles.mainContent}>
                <p className={styles.mainContent}>{post.content}</p>
            </div>

            <div className={styles.information}>
                <div className={styles.author} onClick={clickUserProfileHandler}>
                    <img className={styles.authorImage} src={createAuthorImgURL(post.authorImage)} alt="" />
                    <div>
                        {post.author}
                    </div>
                </div>
                <div className={styles.comments}>
                    <FontAwesomeIcon className={styles.iconComments} icon={faComments} />
                    Comments: <strong>{post.comments.length}</strong>
                </div>
                <div className={styles.postedOn}>
                    {post.postedOn}
                </div>
            </div>
        </div>);
}