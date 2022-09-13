import { useEffect, useReducer } from "react";

import { loadFuturesContracts } from '../../../services/futures/futuresService';

import styles from './FuturesCalculator.module.css';

const initialState = {
    allFutures: [],
    chosenFutures: {name: ''},
};

function futuresReducer(state, action) {
    switch (action.type) {
        case 'loadAllFutures':
            return {
                ...state,
                allFutures: action.allFutures,
                chosenFutures: action.allFutures[0]
            };
        case 'selectContractType':
            return {
                ...state,
                chosenFutures: state.allFutures[action.index]
            };
        default:
            break;
    }
};

export const FuturesCalculator = () => {

    const [futures, dispatch] = useReducer(futuresReducer, initialState);

    useEffect(() => {
        loadFuturesContracts()
        .then(futuresContracts => dispatch({type: 'loadAllFutures', allFutures: futuresContracts}));
    }, []);

    const OnChangeContractType = (value) => {
        dispatch({type: 'selectContractType', index: value})
    };

    return (
        <form className={styles.futuresCalcualtorContainer}>
            <select className={styles.selectContract} onChange={(e) => OnChangeContractType(e.target.value)} select={futures.chosenFutures.name}>
                {futures.allFutures.map((futureContract, index) =>
                    <option key={futureContract.guid} value={index}>{futureContract.name}</option>
                )}
            </select>
            <div className={styles.futuresInformation}>
                <div className={styles.positionInformationContainer}>
                    <div className={styles.directionContainer}>
                        <h4>Position Direction</h4>
                        <div className={styles.directions}>
                            <label>
                                Bullish
                                <input type="radio" checked={futures.durectionBullish} name="directionSelected" />
                            </label>
                            <label>
                                Bearish
                                <input type="radio" checked={!futures.durectionBullish} name="directionSelected" />
                            </label>
                        </div>
                    </div>
                    <div className={styles.entryPriceContainer}>
                        <h4>Entry price</h4>
                        <input min={0} classNAme={styles.numberInput} type="number" />
                    </div>
                    <div className={styles.exitPriceContainer}>
                        <h4>Exit price</h4>
                        <input min={0} classNAme={styles.numberInput} type="number" />
                    </div>
                    <div className={styles.contractsNUmber}>
                        <h4>Number of Contract</h4>
                        <input min={0} classNAme={styles.numberInput} type="number" />
                    </div>
                </div>
                <aside className={styles.contractInformationContainer}>
                    <div className={styles.contractNameContainer}>
                        <h4>
                            {futures.chosenFutures.name}
                        </h4>
                    </div>
                    <div className={styles.tickSizeHeadingContainer}>
                        <h5>Tick Size</h5>
                    </div>
                    <div className={styles.tickSizeValueContainer}>
                        <h5>{futures.chosenFutures.tickSize}</h5>    
                    </div>  
                    <div className={styles.tickValueHeadingContainer}>
                        <h5>Tick Value</h5>    
                    </div>
                    <div className={styles.tickValueContainer}>
                        <h5>${futures.chosenFutures.tickValue}</h5>    
                    </div>
                </aside>
            </div>
        </form>
    );
}