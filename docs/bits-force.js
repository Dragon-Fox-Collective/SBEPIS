"use strict";

const parameters = new URLSearchParams(window.location.search);
const graphWidth = 1920, graphHeight = 1080, graphScale = 3;

const makeParents = parameters.has('makeparents');

(async () => {
	//  Graph data
	let data = await d3.json('bits.json');

	let node_data = [];
	let link_data = [];

	function namespace(nodeId, nodeType)
	{
		return `${nodeType}::${nodeId}`;
	}

	function iterateNode(node, attrs, parentNode, graph, nodeType)
	{
		attrs = Object.assign({}, attrs);

		if ('color' in node)
			attrs.color = node.color;
		
		if ('id' in node && (makeParents || !('nodes' in node)) && ['members', 'bits', 'class', 'items'].includes(nodeType))
		{
			node_data.push(Object.assign({ 'id':namespace(node.id, nodeType), 'name':node.id, 'isParent':'nodes' in node }, attrs));

			if (parentNode !== null && makeParents)
			 	link_data.push({ source: namespace(parentNode.id, parentNode.id === 'base' ? null : nodeType), target: namespace(node.id, nodeType) });
		}

		['bits', 'members', 'items'].forEach(sourceType =>
		{
			if (sourceType in node)
			{
				if (typeof node[sourceType] !== "object")
					throw new Error(namespace(node.id, nodeType) + " uses " + sourceType + " as a " + typeof node[sourceType] + " instead of a object");
				node[sourceType].forEach(source => link_data.push({ source:namespace(source, sourceType), target:namespace(node.id, nodeType) }));
			}
		});

		['class'].forEach(sourceType =>
		{
			if (sourceType in node)
			{
				if (typeof node[sourceType] !== "string")
					throw new Error(namespace(node.id, nodeType) + " uses " + sourceType + " as a " + typeof node[sourceType] + " instead of a string");
				link_data.push({ source:namespace(node[sourceType], sourceType), target:namespace(node.id, nodeType) });
			}
		});

		if ('nodes' in node)
			node.nodes.forEach(child => iterateNode(child, attrs, 'id' in node ? node : parentNode, graph, nodeType === null ? child.id : nodeType));
	}

	iterateNode(data, {}, null, data, null);

	//  SVG elements
	let svg = d3.select('#graph').append('svg')
		.classed('svg', true)
		.attr('viewBox', [0, 0, graphWidth * graphScale, graphHeight * graphScale])
		.attr('width', graphWidth)
		.attr('height', graphHeight);

	let markers = [];
	[true, false].forEach(sourceParent =>
	[true, false].forEach(targetParent =>
		markers.push({
			sourceParent: sourceParent,
			targetParent: targetParent,
		})
	));
	
	const markerBoxSize = 20;
	svg.append('defs').selectAll('marker')
		.data(markers)
		.enter()
		.append('marker')
			.attr('id', marker => 'arrow' + (marker.sourceParent ? '-sourceParent' : '') + (marker.targetParent ? '-targetParent' : ''))
			.attr('viewBox', [0, 0, markerBoxSize, markerBoxSize])
			.attr('refX', marker => markerBoxSize / 2 + (marker.targetParent ? 5 : 10))
			.attr('refY', markerBoxSize / 2)
			.attr('markerWidth', markerBoxSize)
			.attr('markerHeight', markerBoxSize)
			.attr('orient', 'auto-start-reverse')
		.append('path')
			.attr('d', `M ${markerBoxSize / 2} ${markerBoxSize / 2} 0 ${markerBoxSize / 4} 0 ${markerBoxSize * 3 / 4} ${markerBoxSize / 2} ${markerBoxSize / 2}`)
			.attr('fill', marker => marker.sourceParent ? '#00000010' : '#000000')

	let links = svg.selectAll('.link')
		.data(link_data)
		.join('line')
		.classed('link', true);

	let nodes = svg.selectAll('.node')
		.data(node_data)
		.join('g')
		.classed('node', true);
	
	nodes.append('circle')
		.attr('r', node => node.isParent ? 5 : 10)
		.style('fill', node => node.color)
		.append('title')
			.text(node => node.name);
	
	nodes.append('text')
		.text(node => node.name)
		.attr('dx', 0)
		.attr('dy', node => node.isParent ? -5 : -10)
		.attr('text-anchor', 'middle');


	//  Interaction events
	function tick()
	{
		links
			.attr('x1', link => link.source.x)
			.attr('y1', link => link.source.y)
			.attr('x2', link => link.target.x)
			.attr('y2', link => link.target.y);

		nodes
			.attr('transform', node => `translate(${node.x}, ${node.y})`)
			.select('circle')
				.style('stroke', node => node.pinned ? '#000' : null);
	}


	//  Simulation
	let simulation = d3.forceSimulation(node_data)
		.force('charge', d3.forceManyBody().strength(-50))
		.force('center', d3.forceCenter(graphWidth * graphScale / 2, graphHeight * graphScale / 2))
		.force('link', d3.forceLink(link_data).id(node => node.id).distance(20))
		.on('tick', tick);

	nodes
		.call(d3.drag()
			.on('start', (event, node) => {
				if (!event.active) simulation.alphaTarget(0.3).restart();
				node.fx = node.x;
				node.fy = node.y;
			})
			.on('drag', (event, node) => {
				node.fx = event.x;
				node.fy = event.y;
			})
			.on('end', (event, node) => {
				if (!event.active) simulation.alphaTarget(0);
				if (!node.pinned)
				{
					node.fx = null;
					node.fy = null;
				}
			}))
		.on('click', (_event, node) => {
			simulation.restart();
			node.pinned = !node.pinned;
			if (node.pinned)
			{
				node.fx = node.x;
				node.fy = node.y;
			}
			else
			{
				node.fx = null;
				node.fy = null;
			}
		});
	
	
	//  Final attribute and style calls (post forceLink)
	links
		.classed('parent', link => link.source.isParent)
		.attr('marker-end', link => 'url(#arrow' + (link.source.isParent ? '-sourceParent' : '') + (link.target.isParent ? '-targetParent' : '') + ')');

	nodes
		.classed('parent', node => node.isParent)
		.each(node => node.weight = links.filter(link => link.source === node || link.target === node).size());

	simulation.force('link')
		.strength(link => (link.source.isParent ? 0.2 : 1) / Math.min(link.source.weight, link.target.weight))
		.distance(link => link.source.isParent ? 50 : 20);
})();