import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';
import { likePost } from '../../../../services/posts/postsService';
import classNames from 'classnames';
import styles from './PostsLikeWidget.module.css';

export const PostsLikeWidget = ({style,likesCount, postId}) => {

    const likeWidgetClickHandle = (e) => {
        e.preventDefault();
        e.stopPropagation();

        likePost(postId);
    }

    return (
        <div className={classNames(styles.likesContainer, style)} onClick={likeWidgetClickHandle}>
            {likesCount}
            <FontAwesomeIcon className={styles.iconLikes} icon={faThumbsUp} />
        </div>
    );
}