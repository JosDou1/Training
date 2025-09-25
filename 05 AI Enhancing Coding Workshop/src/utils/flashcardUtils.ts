import { Flashcard, FlashcardSet } from '../types';

export const parseCSV = (csvText: string): Flashcard[] => {
  const lines = csvText.split('\n');
  const flashcards: Flashcard[] = [];
  
  for (let i = 0; i < lines.length; i++) {
    const line = lines[i].trim();
    if (!line) continue;
    
    // Handle quoted CSV fields
    const row = line
      .split(/,(?=(?:(?:[^"]*"){2})*[^"]*$)/)
      .map(field => field.replace(/^"|"$/g, '').trim());
    
    if (row.length >= 2) {
      flashcards.push({
        id: crypto.randomUUID(),
        question: row[0],
        answer: row[1],
      });
    }
  }
  
  return flashcards;
};

export const parseJSON = (jsonText: string): Flashcard[] => {
  try {
    const data = JSON.parse(jsonText);
    
    // Handle both array of flashcards and object with cards property
    const cards = Array.isArray(data) ? data : data.cards || [];
    
    return cards.map((card: any) => ({
      id: card.id || crypto.randomUUID(),
      question: card.question || '',
      answer: card.answer || '',
    }));
  } catch (error) {
    throw new Error('Invalid JSON format');
  }
};

export const readFileAsText = (file: File): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    
    reader.onload = (event) => {
      const result = event.target?.result;
      if (typeof result === 'string') {
        resolve(result);
      } else {
        reject(new Error('Failed to read file'));
      }
    };
    
    reader.onerror = () => {
      reject(new Error('Error reading file'));
    };
    
    reader.readAsText(file);
  });
};
