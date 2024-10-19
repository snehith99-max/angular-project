import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUserprivilegereprtComponent } from './sys-mst-userprivilegereprt.component';

describe('SysMstUserprivilegereprtComponent', () => {
  let component: SysMstUserprivilegereprtComponent;
  let fixture: ComponentFixture<SysMstUserprivilegereprtComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUserprivilegereprtComponent]
    });
    fixture = TestBed.createComponent(SysMstUserprivilegereprtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
