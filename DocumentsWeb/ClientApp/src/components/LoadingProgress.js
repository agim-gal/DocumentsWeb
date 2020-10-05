import React, { useState, useEffect } from 'react';
import ProgressBar from 'react-bootstrap/ProgressBar';
import Alert from 'react-bootstrap/Alert';

export function LoadingProgress(props) {
    const [documentProgress, setDocumentProgress] = useState({
        percent: 0,
        status: 1
    });

    useEffect(() => {
        const interval = setInterval(() => {
            fetch('api/Documents/GetDocumentsLoadingInfo')
                .then((response) => {
                    return response.json();
                })
                .then(res => {
                    props.dataUpdateCallback(res.loadingItems)
                    setDocumentProgress(res);
                    if (res.status === 3) {
                        clearInterval(interval);
                    }

                })
        }, 1000);
        return () => clearInterval(interval);
    }, []);

    function resolveProgressBarVariant(val) {
        switch (val) {
            case 1: return 'warning';
            case 2: return 'info';
            case 3: return 'success';
            case 4: return 'danger';
            default: 
        }
    }

    return (
        <div>
            <Alert variant={resolveProgressBarVariant(documentProgress.status)}>{documentProgress.loadingItems} / {documentProgress.allItems}</Alert>
            <ProgressBar striped variant={resolveProgressBarVariant(documentProgress.status)} now={documentProgress.percent} label={`${documentProgress.percent}%`}  />
        </div>
    );
}