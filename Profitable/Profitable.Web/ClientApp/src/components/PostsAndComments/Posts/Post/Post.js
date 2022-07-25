import styles from './Post.module.css';
import { useParams } from 'react';

export const Post = (props) => {

    const { postId } = useParams();

    const createImgURL = () => {
        const imageUrl = `data:image/${props.imageType};base64,${props.image}`;
        return imageUrl;
    }

    return (<div className={styles.post}>
        <div className={styles.content}>
            <h1>{props.title}</h1>
            <p>{props.content}</p>
        </div>
        <div className={styles.information}>
            <div className={styles.author}>
                {props.author}
            </div>
            <div className={styles.postedOn}>
                {props.postedOn}
            </div>
        </div>
    </div>);
}