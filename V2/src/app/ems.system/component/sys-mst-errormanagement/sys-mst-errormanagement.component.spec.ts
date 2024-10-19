import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstErrormanagementComponent } from './sys-mst-errormanagement.component';

describe('SysMstErrormanagementComponent', () => {
  let component: SysMstErrormanagementComponent;
  let fixture: ComponentFixture<SysMstErrormanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstErrormanagementComponent]
    });
    fixture = TestBed.createComponent(SysMstErrormanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
