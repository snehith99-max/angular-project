import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUserAddComponent } from './sys-mst-user-add.component';

describe('SysMstUserAddComponent', () => {
  let component: SysMstUserAddComponent;
  let fixture: ComponentFixture<SysMstUserAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUserAddComponent]
    });
    fixture = TestBed.createComponent(SysMstUserAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
