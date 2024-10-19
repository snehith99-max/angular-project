import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUserEditComponent } from './sys-mst-user-edit.component';

describe('SysMstUserEditComponent', () => {
  let component: SysMstUserEditComponent;
  let fixture: ComponentFixture<SysMstUserEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUserEditComponent]
    });
    fixture = TestBed.createComponent(SysMstUserEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
