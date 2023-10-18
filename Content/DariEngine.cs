using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace c_engine.Content
{
    namespace Globals
    {
        enum States
        {
            None,
            Two,
            Three
        }

    }

    internal class Object : EventListener
    {
        UniqueId id;
        string name;
        string description;
        public UniqueId Id { 
            get { return id; } 
            set { id = value; } 
        }

        public Object()
        {
            this.id = new UniqueId();
            this.name = this.GetType().Name;
        }

        public string get_id()
        {
            return id.ToString();
        }

        public void set_name(string name)
        {
            this.name = name;
        }

    }

    class SceneChild : Object
    {
        private List<SceneChild> children;
        private SceneChild parent = null;
        internal static SceneChild scene_tree = null;

        public SceneChild()
        { 
            this.children = new List<SceneChild>();
        }

        public void add_child(SceneChild child)
        {
            child.parent = this;
            children.Add(child);
        }

        public List<SceneChild> get_children()
        { 
            return children;
        }
        public SceneChild get_parent()
        {
            return parent;
        }

        public SceneChild get_child_at(int index)
        {
            return children[index];
        }

        public void delete_child_at(int index)
        {
            children.RemoveAt(index);
        }

        public void delete_child(SceneChild child)
        {
            children.Remove(child);
        }

        public int get_child_count()
        {  
            return children.Count; 
        }

        public SceneChild get_tree_root()
        {
            return scene_tree;
        }

        internal void update_child(int index)
        {
            //children[index].update();
        }

        internal void draw_child(int index)
        {
            //children[index].draw();
        }


    }

    class Position2D : SceneChild
    {
        public Vector2 position;

        public Position2D()
        {
            this.position = new Vector2();
        }

        public Position2D(Vector2 p_vec)
        {
            this.position = p_vec;
        }


    }


    class Color : Object
    {
        public Vector4 rgba;

        public Color(int p_r, int p_g, int p_b, int p_a)
        {
            this.rgba = new Vector4(p_r,p_g,p_b,p_a);
        }

        public Color()
        {
            this.rgba = new Vector4(1, 1, 1, 1);
        }

        public Color(Vector4 p_color)
        {
            this.rgba = p_color;
        }

        public void set_opacity(int p_opacity)
        {
            this.rgba.W = p_opacity;
        }
    }

    class Spatial : SceneChild
    {
        public Vector2 position;
        public Vector2 global_position;
        public Vector2 rotation;
        public Vector2 global_rotation;
        public Color color;

        public Spatial()
        {
            this.position = new Vector2();
            this.rotation = new Vector2();
            this.global_position = new Vector2();
            this.global_rotation = new Vector2();
        }


        public Vector2 get_position()
        {
            return this.position;
        }

        public void set_position(Vector2 p_position)
        {
            Spatial spatial_parent = get_parent() as Spatial;

            if (spatial_parent == null)
            {
                this.global_position = Vector2.Zero;
            }
            else
            {
                this.global_position = spatial_parent.get_position();
            }

            this.position = this.global_position + p_position;
        }

        public void set_global_position(Vector2 p_global_position)
        {
            this.global_position = p_global_position;
        }

        public Vector2 get_global_position()
        {
            return this.global_position;
        }

    }

    interface Drawable
    {
        abstract public void draw();

    }

    class Rect : Spatial, Drawable
    {
        public Vector2 size;

        public Rect()
        {
            this.size = Vector2.Zero;
        }

        public Rect(Vector2 size)
        {
            this.size = size;
        }

        public Rect(Vector2 size, Vector2 position)
        {
            this.size = size;
        }

        public void draw()
        {

        }

    }

    class Sprite : Rect, Drawable
    {
        private string texture_name;

        public new void draw()
        {

        }

    }

    internal class EventListener
    {
        public void event_received(string func_name)
        {

        }

        public void stop_event(string func_name)
        {

        }

    }

   class Events
    {
        static private List<TimedEvent> timed_events;
        static private Dictionary<string,EventState> conditional_events;
        static Events instance;

        enum EventState
        {
            UNRESOLVED,
            PROCESSING,
            RESOLVED
        }

        public Events()
        {
            Events.instance = this;
        }

        static void add_timed_event(WeakReference<Object> p_connected_object, string p_connectec_func_name, float p_wait_time, bool p_auto_start = true, bool p_repeats = false)
        {
            TimedEvent new_event = new TimedEvent(p_connected_object, p_connectec_func_name, p_wait_time, p_auto_start, p_repeats);
            Events.timed_events.Add(new_event);
        }

        static void add_conditional_event(string p_event_name)
        {
            Events.conditional_events.Add(p_event_name, EventState.UNRESOLVED);
        }

        public void update()
        {
            foreach (TimedEvent timer in Events.timed_events)
            {
                timer.update();
            }

        }
    }

    class TimedEvent : SceneChild
    {
        private float wait_time;
        private float elapsed_time;
        private bool auto_start;
        private bool repeats;
        private bool active;
        private WeakReference<Object> connected_object;
        private string connected_func_name;

        public TimedEvent(WeakReference<Object> p_connected_object, string p_connectec_func_name, float p_wait_time, bool p_auto_start = true, bool p_repeats = false)
        {
            this.wait_time = p_wait_time;
            this.auto_start = p_auto_start;
            this.elapsed_time = 0.0F;
            this.repeats = p_repeats;
            this.connected_func_name = p_connectec_func_name;
            this.connected_object = p_connected_object;

            if (this.auto_start)
            {
                this.active = true;
            }

        }

        public float get_elapsed_time()
        {
            return this.wait_time;
        }

        public void update()
        {
            if (this.active)
            {
               // this.elapsed_time += 1 * App.get_delta_time();
            }

            if (this.elapsed_time >= this.wait_time)
            {
                this.time_out();
            }
         
        }

        public void time_out()
        {
            MethodInfo mi = connected_object.GetType().GetMethod(connected_func_name);
            mi.Invoke(connected_object, null);

            if (this.repeats) {

                this.elapsed_time = this.wait_time;
            }

            else
            {
                this.get_parent().delete_child(this);
            }
        }


    }


    class SceneTree : SceneChild
    {
        public SceneTree()
        {
            SceneChild.scene_tree = this;
        }

    }

}
