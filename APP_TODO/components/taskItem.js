import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

export const TaskItem = ({ task }) => {
    const formattedDate = formatDate(task.dateregister);

    return (
        <View style={styles.task}>
            <Text style={styles.description}>{task.description}</Text>
            <Text style={styles.id}>{task.id}</Text>
            <Text style={styles.date}>{formattedDate}</Text>
        </View>
    );
};

const formatDate = (dateString) => {
    const date = new Date(Date.parse(dateString));
    return date.toLocaleDateString('es-ES', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
};

const styles = StyleSheet.create({
    task: {
        paddingVertical: 15,
        paddingHorizontal: 20,
        borderBottomWidth: 1,
        borderBottomColor: '#ccc',
        backgroundColor: '#fff',
    },
    description: {
        fontSize: 16,
        fontWeight: 'bold',
        marginBottom: 5,
    },
    id: {
        fontSize: 14,
        color: '#666',
        marginBottom: 3,
    },
    date: {
        fontSize: 12,
        color: '#999',
    },
});
