import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstApprovalhierarchyComponent } from './sys-mst-approvalhierarchy.component';

describe('SysMstApprovalhierarchyComponent', () => {
  let component: SysMstApprovalhierarchyComponent;
  let fixture: ComponentFixture<SysMstApprovalhierarchyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstApprovalhierarchyComponent]
    });
    fixture = TestBed.createComponent(SysMstApprovalhierarchyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
