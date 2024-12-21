import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './ItemsTable.css';

const ItemsTable = () => {
    const [items, setItems] = useState([]);
    const [editItemId, setEditItemId] = useState(null);
    const [editedItem, setEditedItem] = useState({ name: '', quantity: '', location: '' });
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        fetchItems();
    }, []);

    const fetchItems = async () => {
        try {
            const response = await axios.get('https://localhost:7202/api/items', {
                withCredentials: true
            });
            setItems(response.data);
        } catch (error) {
            console.error('Error fetching items:', error);
            setError('Failed to fetch items. Please make sure you are logged in.');
        }
    };

    const handleEditClick = (item) => {
        // alert(item.id);
        setEditItemId(item.id);
        setEditedItem({ ...item });
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditedItem((prevItem) => ({ ...prevItem, [name]: value }));
    };

    const handleSaveClick = async () => {
        try {
            await axios.put(`https://localhost:7202/api/items/${editItemId}`, editedItem, {
                withCredentials: true
            });
            setEditItemId(null);
            fetchItems();
        } catch (error) {
            console.error('Error updating item:', error);
            setError('Failed to update item.');
        }
    };

    const handleDeleteClick = async (id) => {
        try {
            await axios.delete(`https://localhost:7202/api/items/${id}`, {
                withCredentials: true
            });
            fetchItems();
        } catch (error) {
            console.error('Error deleting item:', error);
            setError('Failed to delete item.');
        }
    };

    const handleAddItemClick = () => {
        navigate('/item-form');
    };

    return (
        <div className="containers">
            <h2>Items in Inventory</h2>
            {error && <p className="error">{error}</p>}
            <div className="table-wrapper">
                <table className="responsive-table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Quantity</th>
                            <th>Location</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {items.map(item => (
                            <tr key={item.id}>
                                {editItemId === item.id ? (
                                    <>
                                        <td>
                                            <input
                                                type="text"
                                                name="name"
                                                value={editedItem.name}
                                                onChange={handleInputChange}
                                            />
                                        </td>
                                        <td>
                                            <input
                                                type="number"
                                                name="quantity"
                                                value={editedItem.quantity}
                                                onChange={handleInputChange}
                                            />
                                        </td>
                                        <td>
                                            <input
                                                type="text"
                                                name="location"
                                                value={editedItem.location}
                                                onChange={handleInputChange}
                                            />
                                        </td>
                                        <td className="action-buttons">
                                            <button onClick={handleSaveClick}>Save</button>
                                            <button onClick={() => setEditItemId(null)}>Cancel</button>
                                        </td>
                                    </>
                                ) : (
                                    <>
                                        <td>{item.name}</td>
                                        <td>{item.quantity}</td>
                                        <td>{item.location}</td>
                                        <td className="action-buttons">
                                            <button onClick={() => handleEditClick(item)}>Update</button>
                                            <button onClick={() => handleDeleteClick(item.id)} className="delete-button">
                                                Delete
                                            </button>
                                        </td>
                                    </>
                                )}
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
            <button onClick={handleAddItemClick} className="add-button">
                Add New Item
            </button>
        </div>
    );
};

export default ItemsTable;
