//using Microsoft.Xna.Framework;
using SharpDX.Direct3D11;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
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

    public class World
    {
    
        public Events Events = new Events();
        public SceneTree SceneTree = new SceneTree();
        public Physics Physics = new Physics();
        public Sound Sound = new Sound();
        public DeltaTime DeltaTime = new DeltaTime();

        public List<SceneTree> scenes = new List<SceneTree>();

        public World()
        {
            //create scenes 

        }

        public void load_scene(string p_scene_name)
        {
            
        }


    }

    public class MainMenu : SceneChild
    {
        //create main menu setup here
    }

    public class OptionsMenu : SceneChild
    {
        //create main menu setup here
    }

    public class GameWorld : SceneChild
    {
        //create game components here
    }

    public class DeltaTime
    {
        Stopwatch sw = new Stopwatch();
        static DeltaTime instance = new DeltaTime();

        static public long get()
        {
            return DeltaTime.instance.sw.ElapsedTicks / 1000;
        }

        static public void restart()
        {
            DeltaTime.instance.sw.Restart();
        }

        static public void stop()
        {
            DeltaTime.instance.sw.Stop();
        }

    }

    public class Sound
    {
        public Sound()
        {

        }

    }

    public class Screen : Object
    {
        private List<WeakReference<Drawable>> drawable_objects = new List<WeakReference<Drawable>>();
        public Screen()
        {

        }

    }

    public class Object : MethodCoordinator
    {
        UniqueId id;
        string name;

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

    public class SceneChild : Object
    {
        public string child_name = "SceneChild";
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

        public void set_child_name(string p_name)
        {
            this.child_name = p_name;
        } 

        public SceneChild get_child_at(int index)
        {
            return children[index];
        }

        public SceneChild get_child_by_name(string p_name)
        {
            SceneChild child_to_find = children.Find(c => c.child_name == p_name);
            if (child_to_find != null)
            {
                return child_to_find;
            }
            else
            {
                return null;
            }

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


    public class Color : Object
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

    public class Spatial : SceneChild
    {
        public Vector2 position;
        public Vector2 global_position;
        public Vector2 rotation;
        public Vector2 global_rotation;
        public Color color = new Color();

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

    public class Rect : Spatial, Drawable
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

        public Sprite()
        {

        }

        public Sprite(string p_texture_name)
        {

        }

        public void set_texture(string p_texture_name)
        {

        }

        public new void draw()
        {

        }

    }

    interface Dynamic
    {

    } 

    public class Collider : Rect
    {
        WeakReference<SceneChild> owner;
        MoveTypes collision_type = MoveTypes.SLIDE;

        public enum MoveTypes
        {
            STOP,
            SLIDE
        }

        public Collider(Vector2 p_size, WeakReference<SceneChild> p_owner = null)
        {
            this.size = p_size;
            this.owner = p_owner == null ? new WeakReference<SceneChild>(get_parent()) : p_owner;
            Physics.add_collider( new WeakReference<Collider>(this));
        }

        public void collision_detected(Vector2 p_new_position)
        {
            SceneChild out_owner;
            if (this.owner.TryGetTarget(out out_owner))
            {
                //out_owner.collision_detected();
            }

        }

        public MoveTypes get_collision_type()
        {
            return this.collision_type;
        }
        
    }

    public class Physics
    {
        private List<WeakReference<Collider>> all_colliders = new List<WeakReference<Collider>>();
        static Physics instance = new Physics();

        public Physics()
        {

        }

        private void update()
        {
            foreach (WeakReference<Collider> collider in all_colliders)
            {

                Collider collider_ob;
                if (collider.TryGetTarget(out collider_ob))
                {

                    Collider.MoveTypes collider_move_type = collider_ob.get_collision_type();
                   
                    switch (collider_move_type)
                    {
                        case Collider.MoveTypes.STOP:
                            Physics.move_and_stop(collider);
                            break;

                        case Collider.MoveTypes.SLIDE:
                            Physics.move_and_slide(collider);
                            break;
                    }

                }

            }
        }


        static public void move_and_stop(WeakReference<Collider> p_collider)
        {
            //handle collsion logic
        }

        static public void move_and_slide(WeakReference<Collider> p_collider)
        {
            //handle collsion logic
        }

        static public void add_collider(WeakReference<Collider> p_collider)
        {
            Physics.instance.all_colliders.Add(p_collider);
        }
    }

    internal class Navigation
    {

    }

    public class MethodCoordinator
    { 
        public MethodCoordinator()
        {

        }

        public void remove_timed_method(string func_name, WeakReference<MethodCoordinator> p_connected_object = null)
        {
            p_connected_object = get_func_object(p_connected_object);
            TimedMethod event_result = Events.find_timed_event(p_connected_object, func_name);
            Events.delete_event(event_result);
        }

       public void add_timed_method(string p_connectec_func_name, float p_wait_time, bool p_auto_start = true, bool p_repeats = false, WeakReference<MethodCoordinator> p_connected_object = null)
        {
            p_connected_object = get_func_object(p_connected_object);
            TimedMethod new_event = new TimedMethod(p_connected_object, p_connectec_func_name, p_wait_time, p_auto_start, p_repeats);
            Events.add_event(new_event);
        }

        public void pause_timed_method( string p_event_name, WeakReference<MethodCoordinator> p_connected_object = null)
        {
            p_connected_object = get_func_object(p_connected_object);
            TimedMethod event_reuslt = Events.find_timed_event(p_connected_object, p_event_name);
            event_reuslt.pause();
        }

        public void restart_timed_method(string p_event_name, WeakReference<MethodCoordinator> p_connected_object = null)
        {
            p_connected_object = get_func_object(p_connected_object);
            TimedMethod event_reuslt = Events.find_timed_event(p_connected_object, p_event_name);
            event_reuslt.restart();
        }

        static void add_conditional_event(string p_event_name)
        {
            Events.add_event(p_event_name);
        }

        public WeakReference<MethodCoordinator> get_func_object(WeakReference<MethodCoordinator> p_connected_object)
        {
            if (p_connected_object == null)
            {
                return new WeakReference<MethodCoordinator>(this);
            }
            else
            {
                return p_connected_object;
            }
        }

    }

   public class Events
    {
        private List<TimedMethod> timed_methods = new List<TimedMethod>();
        private Dictionary<string,EventState> events = new Dictionary<string, EventState>();
        public static Events instance = new Events();
        
        enum EventState
        {
            UNRESOLVED,
            PROCESSING,
            RESOLVED
        }

        public Events()
        {

        }


        static public void update()
        {
            foreach (TimedMethod timer in Events.instance.timed_methods)
            {
                timer.update();
            }

        }


        static public TimedMethod find_timed_event(WeakReference<MethodCoordinator> p_connected_object, string p_event_name)
        {
            List<TimedMethod> event_results = Events.instance.timed_methods.FindAll(x => x.get_connected_object() == p_connected_object);
            List<TimedMethod> func_name_results = Events.instance.timed_methods.FindAll(x => x.get_func_name() == p_event_name);

            if (func_name_results != null)
            {
                return func_name_results[0];
            }
            else
            {
                return null;
            }
        }

        static public void add_event(TimedMethod p_event)
        {
            Events.instance.timed_methods.Add(p_event);
        }

        static public void add_event(string p_event)
        {
            Events.instance.events.Add(p_event, EventState.UNRESOLVED);
        }

        static public void delete_event(TimedMethod p_event)
        {
            Events.instance.timed_methods.Remove(p_event);
        }

        static public void delete_event(string p_event)
        {
            Events.instance.events.Remove(p_event);
        }




    }

    public class TimedMethod : SceneChild
    {
        private float wait_time;
        private float elapsed_time;
        private bool auto_start;
        private bool repeats;
        private bool active;
        private WeakReference<MethodCoordinator> connected_object;
        private string connected_func_name;

        public TimedMethod(WeakReference<MethodCoordinator> p_connected_object, string p_connectec_func_name, float p_wait_time, bool p_auto_start = true, bool p_repeats = false)
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

        public WeakReference<MethodCoordinator> get_connected_object()
        {
            return this.connected_object;
        }

        public string get_func_name()
        {
            return this.connected_func_name;
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

        public void pause()
        {
            this.active = false;
        }

        public void unpause()
        {
           this.active = true;
        }

        public void restart()
        {
            reset_timer();
            this.active = true;
        }

        public void toggle_repeat()
        {
            this.repeats = !this.repeats;
        }

        private void reset_timer()
        {
            this.elapsed_time = this.wait_time;
        }

        public void time_out()
        {
            Debug.WriteLine("Time Out");
            MethodInfo mi = connected_object.GetType().GetMethod(connected_func_name);
            mi.Invoke(connected_object, null);

            if (this.repeats) {

                restart();
            }

            else
            {
                this.active = false;
            }
        }


    }


    public class SceneTree : SceneChild
    {
        public SceneTree()
        {
            SceneChild.scene_tree = this;
        }

    }

}
