import React, { useState }  from 'react';
import { Layout } from './components/Layout';
import { LoadingProgress } from './components/LoadingProgress';
import { Documents } from './components/Documents';

import './custom.css'

export default function App() {
    const [itemLoadingCount, setItemLoadingCount] = useState(0);
  
    return (
        <Layout>
            <LoadingProgress dataUpdateCallback={(x) => setItemLoadingCount(x)}/>
            <Documents itemLoadingCount={itemLoadingCount} />
        </Layout>
    );
}
