import { useReducer } from "react";


const reducer = (state, action) => {
    switch (action.type) {
        case 'loadPositions':
            return {
                ...state,

            };
    
        default:
            break;
    }
}

export const RecordDetails = ({instrumentTypes}) => {

    const [state, setState] = useReducer(reducer, {
        positions: []
    });

    return (
        <div></div>
    );
}