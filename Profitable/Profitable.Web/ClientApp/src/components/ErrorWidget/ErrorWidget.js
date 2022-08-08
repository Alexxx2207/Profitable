import { CLIENT_ERROR_TYPE } from '../../common/config';

import styles from './ErrorWidget.module.css';

export const ErrorWidget = ({error}) => {
    return (
        <div className={styles.errorContainer}>
            {error.type == CLIENT_ERROR_TYPE ?
                (error.fulfilled ? 
                    <h4 className={styles.errorMessageGreen}>{error.text}</h4>
                :
                    <h4 className={styles.errorMessageRed}>{error.text}</h4>
                )
            :
                (error.display ?
                    <h4 className={styles.errorMessageYellow}>{error.text}</h4>
                :
                    ''
                )
            }
        </div>
    );
}