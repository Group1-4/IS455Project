import { useEffect, useState } from 'react';

const ContentIdList = () => {
  const [contentIds, setContentIds] = useState([]);
  const [selected, setSelected] = useState([]);
  const [recommendations, setRecommendations] = useState({}); // { contentId: {collaborative: [...], contentBased: [...] } }

  useEffect(() => {
    fetch('https://localhost:7118/api/recommendation/all-content-ids')
      .then((res) => res.json())
      .then((data) => setContentIds(data))
      .catch((err) => console.error('Failed to load content IDs', err));
  }, []);

  const handleCheckboxChange = (id) => {
    const isSelected = selected.includes(id);
    const updatedSelected = isSelected
      ? selected.filter((item) => item !== id)
      : [...selected, id];
    setSelected(updatedSelected);

    // If newly selected, fetch both types of recommendations
    if (!isSelected) {
      Promise.all([
        fetch(`https://localhost:7118/api/Recommendation/getCollaborativeRecommendations/${id}`).then((res) =>
          res.json()
        ),
        fetch(`https://localhost:7118/api/Recommendation/getContentRecommendations/${id}`).then((res) =>
          res.json()
        ),
      ])
        .then(([collabRecs, contentRecs]) => {
          setRecommendations((prev) => ({
            ...prev,
            [id]: {
              collaborative: collabRecs,
              contentBased: contentRecs,
            },
          }));
        })
        .catch((err) => console.error('Error fetching recommendations', err));
    }
  };

  return (
    <div className="p-4">
      <h2 className="text-xl font-bold mb-4">Select Content IDs</h2>
      <ul className="space-y-6">
        {contentIds.map((id) => (
          <li key={id}>
            <div className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={selected.includes(id)}
                onChange={() => handleCheckboxChange(id)}
              />
              <label className="font-medium">{id}</label>
            </div>

            {recommendations[id] && (
              <div className="ml-6 mt-2 text-sm">
                <div>
                  <strong>Collaborative:</strong>{' '}
                  {recommendations[id].collaborative.join(', ')}
                </div>
                <div>
                  <strong>Content-Based:</strong>{' '}
                  {recommendations[id].contentBased
                    .map((item) => item.contentId || item.title) // Adjust property here based on your response structure
                    .join(', ')}
                </div>
              </div>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ContentIdList;
