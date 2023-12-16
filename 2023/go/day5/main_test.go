package main

import (
	"slices"
	"testing"
)

func TestUpdateRange(t *testing.T) {
	mappings := []Mapping{
		{50, 98, 2},
		{52, 50, 48},
	}

	r := Range{79, 14}
	newRange := updateRange(mappings, r)
	if newRange.start != 81 {
		t.Errorf("Expected start to be 81, got %d", newRange.start)
	}
	if newRange.length != 14 {
		t.Errorf("Expected length to be 14, got %d", newRange.length)
	}

	mappings = []Mapping{
		{50, 98, 2},
		{52, 50, 48},
	}

	r = Range{79, 14}
	newRange = updateRange(mappings, r)
	if newRange.start != 81 {
		t.Errorf("Expected start to be 81, got %d", newRange.start)
	}
	if newRange.length != 14 {
		t.Errorf("Expected length to be 14, got %d", newRange.length)
	}

	mappings = []Mapping{
		{3, 3, 2},
	}

	// perfect range
	r = Range{3, 2}
	updatedRange := updateRange(mappings, r)
	if updatedRange.start != 3 {
		t.Errorf("Expected start to be 3, got %d", updatedRange.start)
	}
	if updatedRange.length != 2 {
		t.Errorf("Expected length to be 2, got %d", updatedRange.length)
	}

	// partial range - take the overlap
	r = Range{5, 5}
	updatedRange = updateRange(mappings, r)
	if updatedRange.start != 5 {
		t.Errorf("Expected start to be 5, got %d", updatedRange.start)
	}
	if updatedRange.length != 5 {
		t.Errorf("Expected length to be 5, got %d", updatedRange.length)
	}

	// partial range - too low - trim the bottom
	r = Range{0, 5}
	updatedRange = updateRange(mappings, r)
	if updatedRange.start != 0 {
		t.Errorf("Expected start to be 0, got %d", updatedRange.start)
	}
	if updatedRange.length != 3 {
		t.Errorf("Expected length to be 3, got %d", updatedRange.length)
	}

	// outside range - too high - return the original range
	r = Range{10, 5}
	updatedRange = updateRange(mappings, r)
	if updatedRange.start != 10 {
		t.Errorf("Expected start to be 10, got %d", updatedRange.start)
	}
	if updatedRange.length != 5 {
		t.Errorf("Expected length to be 5, got %d", updatedRange.length)
	}

	// outside range - too low - return the original range
	r = Range{0, 2}
	updatedRange = updateRange(mappings, r)
	if updatedRange.start != 0 {
		t.Errorf("Expected start to be 0, got %d", updatedRange.start)
	}
	if updatedRange.length != 2 {
		t.Errorf("Expected length to be 2, got %d", updatedRange.length)
	}

}

func TestMapRange(t *testing.T) {
	mappings := []Mapping{
		{49, 53, 8},
		{0, 11, 42},
		{42, 0, 7},
		{57, 7, 4},
	}

	ranges := []Range{
		{57, 13},
	}

	updatedRanges := mapRange(mappings, ranges)

	if len(updatedRanges) != 2 {
		t.Errorf("Expected 2 range, got %d", len(updatedRanges))
	}
	if updatedRanges[0].start != 53 {
		t.Errorf("Expected start to be 53, got %d", updatedRanges[0].start)
	}
	if updatedRanges[0].length != 4 {
		t.Errorf("Expected length to be 4, got %d", updatedRanges[0].length)
	}
	if updatedRanges[1].start != 61 {
		t.Errorf("Expected start to be 61, got %d", updatedRanges[1].start)
	}
	if updatedRanges[1].length != 9 {
		t.Errorf("Expected length to be 9, got %d", updatedRanges[1].length)
	}

	mappings = []Mapping{
		{5, 5, 1},
	}
	ranges = []Range{
		{0, 10},
	}

	updatedRanges = mapRange(mappings, ranges)

	if len(updatedRanges) != 3 {
		t.Errorf("Expected 3 ranges, got %d", len(updatedRanges))
	}
	if slices.Contains(updatedRanges, Range{0, 5}) != true {
		t.Errorf("Expected range to contain [0, 5] but got %v", updatedRanges)
	}
	if slices.Contains(updatedRanges, Range{5, 1}) != true {
		t.Errorf("Expected range to contain [5, 1] but got %v", updatedRanges)
	}
	if slices.Contains(updatedRanges, Range{6, 4}) != true {
		t.Errorf("Expected range to contain [6, 4] but got %v", updatedRanges)
	}

	//testing with multiple mappings
	mappings = []Mapping{
		{10, 5, 5},
		{20, 3, 2},
	}

	ranges = []Range{
		{3, 7},
	}
	updatedRanges = mapRange(mappings, ranges)

	if len(updatedRanges) != 2 {
		t.Errorf("Expected 2 ranges, got %d", len(updatedRanges))
	}
	if slices.Contains(updatedRanges, Range{20, 2}) != true {
		t.Errorf("Expected range to be [20, 2]")
	}
	if slices.Contains(updatedRanges, Range{10, 5}) != true {
		t.Errorf("Expected range to be [10, 5]")
	}
}
