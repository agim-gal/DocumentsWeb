import React, { useState, useEffect } from 'react';
import Pagination from 'react-bootstrap/Pagination'
import FormControl from 'react-bootstrap/FormControl'

export function Documents(props) {
    const [data, setData] = useState({ documents: [], allCount: 0 });
    const [pagination, setPagination] = useState({ page: 1, size: 10, term: '' });
    const [showDocumentList, setShowDocumentList] = useState([]);

    function setPage(page) {
        setPagination({ page: page, size: pagination.size, term: pagination.term });
    }

    function setTerm(term) {
        setPagination({ page: 1, size: pagination.size, term: term });
    }

    useEffect(() => {
        fetch('api/Documents/?' + new URLSearchParams(pagination))
        .then((response) => {
            return response.json();
        })
        .then(res => {
            setData({ documents: res.list, allCount: res.allCount });
        })
    }, [pagination, props.itemLoadingCount])

    function toggleContent(val) {
        var res;
        if (showDocumentList.indexOf(val) === -1) {
            res = showDocumentList.concat(val);
        } else {
            res = showDocumentList.filter(x => x !== val);
        }
        setShowDocumentList(res);
    }

    function reanderPagination() {
        var pageCount = Math.ceil(data.allCount / pagination.size)
        var pageButtons = [];
        if (pageCount < 10) {
            for (var i = 1; i < pageCount + 1; i++) {
                pageButtons.push({ page: i, active: i === pagination.page, mod: 'Item' });
            }
        } else {
            var needLeftDots = true;
            var leftBounds = 0;
            if (pagination.page < 10) {
                needLeftDots = false;
                leftBounds = 1;
            }
            else {
                leftBounds = pagination.page - 4;
            }

            var needRightDots = true;
            var rightBounds = 0;
            if (pagination.page > pageCount - 10) {
                needRightDots = false;
                rightBounds = pageCount;
            } else {
                rightBounds = Math.max(pagination.page + 4, 10);
            }

            pageButtons.push({ page: 1, active: false, disabled: !needLeftDots, mod: 'First' });
            pageButtons.push({ page: pagination.page - 1, active: false, disabled: pagination.page === 1, mod: 'Prev' });
            if (needLeftDots) {
                pageButtons.push({ page: 1, active: false, mod: 'Item' });
                pageButtons.push({ active: false, mod: 'Ellipsis' });
            }

            for (var j = leftBounds; j <= rightBounds; j++) {
                pageButtons.push({ page: j, active: j === pagination.page, mod: 'Item' });
            }
            
            if (needRightDots) {
                pageButtons.push({ active: false, mod: 'Ellipsis' });
                pageButtons.push({ page: pageCount, active: false, mod: 'Item' });
            }
            pageButtons.push({ page: pagination.page + 1, active: false, disabled: pagination.page === pageCount, mod: 'Next' });
            pageButtons.push({ page: pageCount, active: false, disabled: !needRightDots, mod: 'Last' });
        }

        return (
            <Pagination>
                {pageButtons.map(p => {
                    switch (p.mod) {
                        case 'Item': return (<Pagination.Item onClick={() => setPage(p.page)} active={p.active}>{p.page}</Pagination.Item>)
                        case 'First': return (<Pagination.First onClick={() => setPage(p.page)} disabled={p.disabled}/>)
                        case 'Prev': return (<Pagination.Prev onClick={() => setPage(p.page)} disabled={p.disabled}/>)
                        case 'Ellipsis': return (<Pagination.Ellipsis />)
                        case 'Next': return (<Pagination.Next onClick={() => setPage(p.page)} disabled={p.disabled}/>)
                        case 'Last': return (<Pagination.Last onClick={() => setPage(p.page)}disabled={p.disabled}/>)
                        default: return null;
                    }
                })}
            </Pagination>
        );
    }

    return (
        <div class="mt-4">
          <FormControl placeholder="Start typing the document title" onChange={(x) => setTerm(x.target.value)}/>
          <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th>Name</th>
              </tr>
            </thead>
            <tbody>
              {data.documents.map(d => 
                <React.Fragment>
                    <tr key={d.id} onClick={() => toggleContent(d.id)}>
                      <td>{d.name}</td>
                    </tr>
                      <tr key={'content_' + d.id} hidden={showDocumentList.indexOf(d.id) === -1} >
                          <td>
                              {d.documentContent.decisionOnCase.split('\n').map(s => 
                                  <p>{s}</p>
                              )}
                          </td>
                    </tr>
                </React.Fragment>
              )}
            </tbody>
          </table>
            { reanderPagination() }
        </div>
    );
}
