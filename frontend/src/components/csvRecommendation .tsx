import React, { useEffect, useState } from 'react';

const ContentIdSelector = () => {
  const [contentIds, setContentIds] = useState([]);
  const [selected, setSelected] = useState([]);

  useEffect(() => {
    fetch('http://localhost:5226/api/recommendation/all-content-ids')
      .then((res) => {
        if (!res.ok) {
          throw new Error('Failed to fetch content IDs');
        }
        return res.json();
      })
      .then((data) => setContentIds(data))
      .catch((err) => console.error(err));
  }, []);

  const handleCheckboxChange = (id) => {
    setSelected((prev) =>
      prev.includes(id) ? prev.filter((item) => item !== id) : [...prev, id]
    );
  };

  return (
    <div className="p-4">
      <h2 className="text-xl font-bold mb-4">Select Content IDs</h2>
      <ul className="space-y-2">
        {contentIds.map((id) => (
          <li key={id} className="flex items-center space-x-2">
            <input
              type="checkbox"
              checked={selected.includes(id)}
              onChange={() => handleCheckboxChange(id)}
            />
            <label>{id}</label>
          </li>
        ))}
      </ul>
      <div className="mt-4">
        <strong>Selected IDs:</strong> {selected.join(', ')}
      </div>
    </div>
  );
};

export default ContentIdSelector;
